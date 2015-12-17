namespace Elasticsearch.Net
{
	//TODO SNIFFONFAIL AND SKIPNODE ARE NEVER USED
	public enum AuditEvent
	{
		SniffOnStartup,
		SniffOnFail,
		SniffOnStaleCluster,

		SniffSuccess,
		SniffFailure,
		PingSuccess,
		PingFailure,

		Resurrection,
		AllNodesDead,
		BadResponse,
		HealthyResponse,

		MaxTimeoutReached,
		MaxRetriesReached
	}
}