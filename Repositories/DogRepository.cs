using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get { return new SqlConnection(_config.GetConnectionString("DefaultConnection")); }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"
                        SELECT d.Id, d.[Name], d.OwnerId, d.Breed, d.Notes,
                        d.ImageUrl, o.[Name] as OwnerName
                        FROM Dog d 
                        LEFT JOIN Owner o ON d.OwnerId = o.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"
                        SELECT d.Id, d.[Name], d.OwnerId, d.Breed, d.Notes,
                        d.ImageUrl, o.[Name] as OwnerName
                        FROM Dog d 
                        LEFT JOIN Owner o ON d.OwnerId = o.Id
                        WHERE d.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        reader.Close();
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"
                    INSERT INTO Dog ([Name], OwnerId, Breed, Notes, ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @ownerId, @breed, @notes, @imageUrl)";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl ?? (object)DBNull.Value);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Dog
                        SET
                        [Name] = @name,
                        OwnerId = @ownerId,
                        Breed = @breed,
                        Notes = @notes,
                        ImageUrl = @imageUrl
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
