using Data.BLL.Migration;
using System.Threading.Tasks;

namespace Data.Config
{
    public static class MigrationConfig
    {
        public static void Migrate()
        {
            RoleMigration.Migrate();
            PaymentMethodMigration.Migrate();
            UserMigration.Migrate();
        }

        public static async Task MigrateAsync()
        {
            await RoleMigration.MigrateAsync();
            await PaymentMethodMigration.MigrateAsync();
            await UserMigration.MigrateAsync();
        }
    }
}
