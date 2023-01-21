using Newtonsoft.Json;
using Telephones.API.Data.Models;

namespace Telephones.API.Data
{
    /// <summary>
    /// Первоначальная заполнение базы данных
    /// </summary>
    public static class DataInitializer
    {
        /// <summary>
        /// Заполняет базу данных начальными значениями если она пуста
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task InitializerAsync(IServiceProvider serviceProvider)
        {

            IServiceScope scope = serviceProvider.CreateScope();
            await using AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();

            if (context is not null || context!.Records.Count() != 0) return;

            using (HttpClient client = new HttpClient())
            {
                string s = client.GetStringAsync(@"https://api.randomdatatools.ru/?unescaped=false&count=20").Result;
                foreach (Dictionary<string, string> record in JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(s)!)
                {
                    context.Records.Add(new Record()
                    {
                        FirstName = record["FirstName"],
                        LastName = record["LastName"],
                        FatherName = record["FatherName"],
                        PhoneNumber = record["Phone"],
                        Address = record["Address"],
                        Discript = record["EduSpecialty"]
                    });
                }

                context.SaveChanges();
            }
        }
    }
}