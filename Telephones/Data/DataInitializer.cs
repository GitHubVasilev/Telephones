using Newtonsoft.Json;
using Telephones.Data.Models;

namespace Telephones.Data
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

            var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<AppDbContext>();

            if (context.Records.Count() != 0) return;

            using (HttpClient client = new HttpClient())
            {
                string s = client.GetStringAsync(@"https://api.randomdatatools.ru/?unescaped=false&count=20").Result;
                foreach (Dictionary<string, string> record in JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(s))
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
