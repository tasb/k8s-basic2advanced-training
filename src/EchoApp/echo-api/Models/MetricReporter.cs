using Prometheus;

namespace echo_api.Models
{
    public class MetricReporter
    {
        private readonly ILogger<MetricReporter> _logger;
        private readonly Counter _requestCounter;
        public MetricReporter(ILogger<MetricReporter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _requestCounter =
                Metrics.CreateCounter("echo_app_api_echo_total_requests", "The total number of requests for echo endpoint.");
        }
        public void RegisterRequest()
        {
            _requestCounter.Inc();
        }
    }
}