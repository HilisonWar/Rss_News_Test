
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Rss_Application.Models;

namespace NewsRSS_TestExersice.Helpers
{
	public  class RssService
	{
		private readonly PostgresContext db;

		public RssService(PostgresContext db)
		{
			this.db = db;
		}

		/// <summary>
		/// Получение данных по новостям на основе URL RSS канала
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public  List<News> GetNewsFromRssByUrl(string url) 
		{
			try
			{
				string xmlStr = DownloadXmlByUrl(url);

				XmlDocument xDoc = new XmlDocument();

				xDoc.LoadXml(xmlStr);

				List<News> newsFromRss = ParseXmlToNews(xDoc);

				return newsFromRss;
			}
			catch (Exception ex)
			{
				return new List<News>();
			}

		}

		/// <summary>
		/// Парсинг полученного XML документа и получение необходимых данных по новостям
		/// </summary>
		/// <param name="xmlDocument"></param>
		/// <returns></returns>
		private  List<News> ParseXmlToNews(XmlDocument xmlDocument)
		{
			try
			{
				XmlElement? xRoot = xmlDocument.DocumentElement;

				News currentNewsFromXml = new News();

				List<News> allNewsFromXml = new List<News>();

				if (xRoot != null)
				{
					foreach (XmlElement xmlElement in xRoot)
					{
						foreach (XmlNode xmlNode in xmlElement.ChildNodes)
						{
							if (xmlNode.Name == "item")
							{
								currentNewsFromXml = new News();

								foreach (XmlElement item_data in xmlNode.ChildNodes)
								{
									switch (item_data.Name)
									{
										case "title":
											currentNewsFromXml.Title = item_data.InnerText;
											break;
										case "link":
											currentNewsFromXml.Url = item_data.InnerText;
											break;
										case "description":
											currentNewsFromXml.Body = item_data.InnerText;
											break;
									}
								}
								currentNewsFromXml.AddingDate = DateTime.Now;
								allNewsFromXml.Add(currentNewsFromXml);
							}
						}
					}
				}

				return allNewsFromXml;
			}
			catch (Exception ex)
			{
				return new List<News>();
			}
		}

		/// <summary>
		/// Загрузка XML файл по URL и возрат в строковом представлении
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		private  string DownloadXmlByUrl(string url)
		{
			try
			{
				using (var wc = new WebClient())
				{
					return wc.DownloadString(url);
				}
			}catch(Exception ex)
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// Добавление новостных записей в таблицу в БД
		/// </summary>
		/// <param name="news"></param>
		public  void AddNewsToDb(List<News> news)
		{
			if (news.Count > 0)
			{
				db.News.AddRange(news);

				db.SaveChanges();
			}
		}
	}
}
