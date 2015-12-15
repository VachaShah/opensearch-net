﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elasticsearch.Net;

namespace Tests.Framework.MockResponses
{
	public static class SniffResponse
	{
		private static string ClusterName => "elasticsearch-test-cluster";

		public static byte[] Create(IEnumerable<Node> nodes)
		{
			var response = new
			{
				cluster_name = ClusterName,
				nodes = SniffResponseNodes(nodes)
			};
			using (var ms = new MemoryStream())
			{
				new ElasticsearchDefaultSerializer().Serialize(response, ms);
				return ms.ToArray();
			}
		}
		private static IDictionary<string, object> SniffResponseNodes(IEnumerable<Node> nodes) =>
			(from node in nodes
			let id = string.IsNullOrEmpty(node.Id) ? Guid.NewGuid().ToString("N").Substring(0, 8) : node.Id
			let name = string.IsNullOrEmpty(node.Name) ? Guid.NewGuid().ToString("N").Substring(0, 8) : node.Name
			select new { id, name, node })
			.ToDictionary(kv => kv.id, kv => CreateNodeResponse(kv.node, kv.name));

		private static object CreateNodeResponse(Node node, string name)
		{
			var nodeResponse = new
			{
				name = name,
				transport_address = $"127.0.0.1:{node.Uri.Port + 1000}]",
				http_address = $"127.0.0.1:{node.Uri.Port}",
				host = Guid.NewGuid().ToString("N").Substring(0, 8),
				ip = "127.0.0.1",
				version = TestClient.ElasticsearchVersion,
				build = Guid.NewGuid().ToString("N").Substring(0, 8),
				settings = new Dictionary<string, object> {
					{ "cluster.name", ClusterName },
					{ "name", name },
				}
			};
			if (!node.MasterEligable) nodeResponse.settings.Add("node.master", false);
			if (!node.HoldsData) nodeResponse.settings.Add("node.data", false);
			return nodeResponse;
		}

	}
}
