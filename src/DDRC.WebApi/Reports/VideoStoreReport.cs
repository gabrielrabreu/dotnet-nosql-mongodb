using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using MongoDB.Driver;

namespace DDRC.WebApi.Reports
{
    public interface IVideoStoreReport
    {
        VideoStoreReportsDto? Generate();
    }

    public class VideoStoreReport : IVideoStoreReport
    {
        private const int MIN_DAYS_RANGE = 5;
        private const int MAX_DAYS_RANGE = 5;

        private readonly MongoDbContext _context;

        private List<VideoStoreModel> _videoStores = new();
        private List<MovieModel> _movies = new();
        private List<FulfilledSaleModel> _fulfilledSales = new();
        private List<ExpectedSaleModel> _expectedSales = new();
        private List<StockModel> _stocks = new();

        private DateTimeOffset _currentDateTime;
        private DateTimeOffset _initialDateTime;
        private DateTimeOffset _endDateTime;

        public VideoStoreReport(MongoDbContext context)
        {
            _context = context;
        }

        public VideoStoreReportsDto? Generate()
        {
            _currentDateTime = DateTime.UtcNow.Date;
            _initialDateTime = _currentDateTime.AddDays(-MIN_DAYS_RANGE);
            _endDateTime = _currentDateTime.AddDays(MAX_DAYS_RANGE);

            _videoStores = _context.VideoStores.Find(x => true).ToList();
            _movies = _context.Movies.Find(x => true).ToList();
            _fulfilledSales = _context.FulfilledSales.Find(x => true).ToList();
            _expectedSales = _context.ExpectedSales.Find(x => true).ToList();
            _stocks = _context.Stocks.Find(x => true).ToList();

            var result = new VideoStoreReportsDto();

            foreach (var videoStore in _videoStores)
            {
                result.VideoStores.Add(new VideoStoreReportDto
                {
                    VideoStore = videoStore.Name,
                    Movies = MapMovies(videoStore)
                });
            }

            return result;
        }

        private List<MovieSalesReportDto> MapMovies(VideoStoreModel videoStore)
        {
            var result = new List<MovieSalesReportDto>();

            foreach (var movie in _movies)
            {
                result.Add(MapMovie(videoStore, movie));
            }

            return result;
        }

        private MovieSalesReportDto MapMovie(VideoStoreModel videoStore, MovieModel movie)
        {
            var result = new MovieSalesReportDto()
            {
                Movie = movie.Title,
                Days = MapMovieDays(videoStore, movie),
                RetroactiveDays = MapMovieRetroactiveDays(videoStore, movie)
            };

            return result;
        }

        private List<DayMovieSalesReportDto> MapMovieDays(VideoStoreModel videoStore, MovieModel movie)
        {
            var result = new List<DayMovieSalesReportDto>();

            if (videoStore == null) return result;

            var stockOnDay = _stocks
                .SingleOrDefault(x => x.Movie.Id == movie.Id
                                   && x.Date == _currentDateTime)?.Amount ?? 0;

            for (DateTimeOffset date = _currentDateTime; date < _endDateTime; date = date.AddDays(1))
            {
                var allMovieSalesOnDay = _expectedSales
                    .Where(x => x.Movie.Id == movie.Id
                             && x.Date == date);

                var movieSalesOnDayAndVideoStore = _expectedSales
                    .SingleOrDefault(x => x.Movie.Id == movie.Id
                                       && x.VideoStore.Id == videoStore.Id
                                       && x.Date == date);

                result.Add(new DayMovieSalesReportDto()
                {
                    Date = date,
                    Stock = stockOnDay,
                    SalesOnAllVideoStores = allMovieSalesOnDay.Sum(x => x.Amount),
                    SalesOnCurrentVideoStore = movieSalesOnDayAndVideoStore?.Amount ?? 0
                });

                stockOnDay -= allMovieSalesOnDay.Sum(x => x.Amount);
            }

            return result;
        }

        private List<DayMovieSalesReportDto> MapMovieRetroactiveDays(VideoStoreModel videoStore, MovieModel movie)
        {
            var result = new List<DayMovieSalesReportDto>();

            if (videoStore == null) return result;

            for (DateTimeOffset date = _initialDateTime; date < _currentDateTime; date = date.AddDays(1))
            {
                var stockOnDay = _stocks
                    .SingleOrDefault(x => x.Movie.Id == movie.Id
                                       && x.Date == date)?.Amount ?? 0;

                var allMovieSalesOnDay = _fulfilledSales
                    .Where(x => x.Movie.Id == movie.Id
                             && x.Date.Date == date.Date);

                var movieSalesOnDayAndVideoStore = _fulfilledSales
                    .Where(x => x.Movie.Id == movie.Id
                             && x.VideoStore.Id == videoStore.Id
                             && x.Date.Date == date.Date);

                result.Add(new DayMovieSalesReportDto()
                {
                    Date = date,
                    Stock = stockOnDay,
                    SalesOnAllVideoStores = allMovieSalesOnDay.Count(),
                    SalesOnCurrentVideoStore = movieSalesOnDayAndVideoStore.Count()
                });
            }

            return result;
        }
    }
}
