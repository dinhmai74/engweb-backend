using Microsoft.EntityFrameworkCore;
using HollypocketBackend.DTO;

namespace HollypocketBackend.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}