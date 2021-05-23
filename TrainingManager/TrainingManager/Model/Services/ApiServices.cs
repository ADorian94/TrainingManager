using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;

namespace TrainingManager.Model.Services
{
    public class ApiServices
    {
        private HttpClient _client;

        public ApiServices(string baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        //WORKOUTS
        public async Task<IEnumerable<WeightWorkoutDTO>> GetWeightWorkoutsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<WeightWorkoutDTO>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/WeightWorkouts", weigthWorkoutDto);
                weigthWorkoutDto.Id = (await response.Content.ReadAsAsync<WeigthExerciseDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        //EXERCISES
        public async Task<IEnumerable<WeigthExerciseDTO>> GetWeightExercisesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/weightexercises/");

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<WeigthExerciseDTO> exercises = await response.Content.ReadAsAsync<IEnumerable<WeigthExerciseDTO>>();
                    return exercises;
                }
                else
                {
                    throw new Exception("error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddWeightExerciseAsync(WeigthExerciseDTO weigthExerciseDto)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/WeightExercises", weigthExerciseDto);
                weigthExerciseDto.Id = (await response.Content.ReadAsAsync<WeigthExerciseDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateWeightExerciseAsync(WeigthExerciseDTO weigthExerciseDto)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/WeightExercises", weigthExerciseDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }
    }
}
