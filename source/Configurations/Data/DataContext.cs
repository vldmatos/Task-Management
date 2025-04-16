using Microsoft.EntityFrameworkCore;

namespace Configurations.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{ }
