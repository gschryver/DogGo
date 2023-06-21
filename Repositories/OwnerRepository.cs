using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT o.Id, o.Email, o.[Name], o.Address, o.NeighborhoodId, o.Phone, n.Name AS NeighborhoodName
                    FROM Owner o
                    LEFT JOIN Neighborhood n ON n.Id = o.NeighborhoodId
                ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.IsDBNull(reader.GetOrdinal("NeighborhoodId"))
                                             ? (int?)null
                                             : reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Neighborhood = new Neighborhood
                            {
                                Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                            }
                        };

                        owners.Add(owner);
                    }


                    reader.Close();

                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT o.Id, o.Email, o.[Name], o.Address, o.NeighborhoodId, o.Phone, n.Name AS NeighborhoodName,
                           d.Id AS DogId, d.Name AS DogName, d.Breed, d.Notes, d.ImageUrl
                    FROM Owner o
                    LEFT JOIN Neighborhood n ON n.Id = o.NeighborhoodId
                    LEFT JOIN Dog d ON d.OwnerId = o.Id
                    WHERE o.Id = @id
                    ";


                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Owner owner = null;

                    while (reader.Read())
                    {
                        if (owner == null)
                        {
                            owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                NeighborhoodId = reader.IsDBNull(reader.GetOrdinal("NeighborhoodId"))
                                                 ? (int?)null
                                                 : reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                },
                                Dogs = new List<Dog>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Notes")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                           ? null
                                           : reader.GetString(reader.GetOrdinal("ImageUrl")),
                                OwnerId = owner.Id
                            };

                            owner.Dogs.Add(dog);
                        }
                    }

                    reader.Close();

                    return owner;
                }
            }
        }
    }
 }

