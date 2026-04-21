using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly string _connectionString;

    public UsuariosController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // ==================== PRUEBA ====================
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { mensaje = "✅ API de Usuarios funcionando correctamente" });
    }

    // ==================== OBTENER TODOS LOS USUARIOS ====================
    [HttpGet]
    public IActionResult GetTodos()
    {
        var lista = new List<object>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Nombre, Telefono, Celular, Email FROM Usuarios";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new
                            {
                                id = reader["Id"],
                                nombre = reader["Nombre"]?.ToString(),
                                telefono = reader["Telefono"]?.ToString(),
                                celular = reader["Celular"]?.ToString(),
                                email = reader["Email"]?.ToString()
                            });
                        }
                    }
                }
            }

            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al obtener usuarios", error = ex.Message });
        }
    }

    // ==================== CREAR NUEVO USUARIO ====================
    [HttpPost]
    public IActionResult Crear([FromBody] System.Text.Json.JsonElement data)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                INSERT INTO Usuarios (Nombre, Telefono, Celular, Email) 
                VALUES (@Nombre, @Telefono, @Celular, @Email)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", data.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "");
                    cmd.Parameters.AddWithValue("@Telefono", data.TryGetProperty("telefono", out var t) ? t.GetString() ?? "" : "");
                    cmd.Parameters.AddWithValue("@Celular", data.TryGetProperty("celular", out var c) ? c.GetString() : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", data.TryGetProperty("email", out var e) ? e.GetString() : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            return Ok(new { mensaje = "Usuario creado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = "Error al crear usuario", error = ex.Message });
        }
    }
}