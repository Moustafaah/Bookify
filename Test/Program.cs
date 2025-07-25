using Dapper;

using Infrastructure.Outbox;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
namespace Test;


internal class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "Server=.;Database=Bookify;Trusted_Connection=True;TrustServerCertificate=True;Application Name=Contoso;";
        SqlTableDependency<OutboxMessage> _tableDependency = new SqlTableDependency<OutboxMessage>(connectionString, "outbox_messages");
        _tableDependency.OnChanged += OnChanged;
        _tableDependency.OnError += (s, e) => Console.WriteLine($"Error: {e.Error.Message}");
        _tableDependency.Start();

        Console.WriteLine("Starting SqlTableDependency...");

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {

                connection.Open();

                var user = connection.QuerySingle<string>("SELECT CURRENT_USER");
                await connection.ExecuteAsync(@"
                                   INSERT INTO outbox_messages (Id, OccuredOn, Type, Content)
                                    VALUES (@Id, @OccuredOn, @Type, @Content)",
                    new
                    {
                        Id = Guid.NewGuid(),
                        OccuredOn = DateTime.UtcNow,
                        Type = "UserCreated",
                        Content = "{\"name\":\"Jake\"}"
                    });
                Console.WriteLine($"Connected as: {user}");


                Console.WriteLine("Waiting for change...");
                await Task.Delay(TimeSpan.FromSeconds(20));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        _tableDependency.Stop();
        _tableDependency.Dispose();


        //string currentUser = WindowsIdentity.GetCurrent().Name;
        //Console.WriteLine($"Current Windows User: {currentUser}");





    }

    private static void OnChanged(object sender, RecordChangedEventArgs<OutboxMessage> e)
    {
        Console.WriteLine(
            $"{nameof(ProcessOutboxMessages)}: process message {e.Entity.Id}: {e.Entity.Type}");
    }


}
