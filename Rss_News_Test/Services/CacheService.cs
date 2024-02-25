using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace NewsRSS_TestExersice.Services
{
	public class CacheService
	{
		private readonly IDistributedCache cache;

		public CacheService(IDistributedCache cache)
		{
			this.cache = cache;

		}

		/// <summary>
		/// Получение данных из кеша по ключу
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Get<T>(string key)
		{
			string cache_data = cache.GetString(key);

			if (string.IsNullOrEmpty(cache_data))
				return default;

			else return JsonConvert.DeserializeObject<T>(cache_data);
		}

		/// <summary>
		/// Запись данных в кеш
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Add<T>(string key, T value)
		{
			string data = JsonConvert.SerializeObject(value);

			cache.SetString(key, data, new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
			});
		}

	}
}
