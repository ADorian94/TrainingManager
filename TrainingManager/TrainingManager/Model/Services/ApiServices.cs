using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;

namespace TrainingManager.Model.Services
{
    public class ApiServices
    {
        private readonly HttpClient _client;

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
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/WeightWorkouts/", weigthWorkoutDto);
                var id = (await response.Content.ReadAsAsync<WeightWorkoutDTO>()).Id;
                weigthWorkoutDto.Id = id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> EditWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/WeightWorkouts", weigthWorkoutDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteWeightWorkoutAsync(int workoutId)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"api/WeightWorkouts/{workoutId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        //ACTIVITIES
        public async Task<IEnumerable<string>> GetWeightActivitiesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightActivities");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<string>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //EXERCISES
        public async Task<IEnumerable<WeightExerciseDTO>> GetWeightExercisesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/weightexercises/");

                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<WeightExerciseDTO> exercises = await response.Content.ReadAsAsync<IEnumerable<WeightExerciseDTO>>();
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

        public async Task<bool> AddWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/WeightExercises", weigthExerciseDto);
                weigthExerciseDto.Id = (await response.Content.ReadAsAsync<WeightExerciseDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //todo: saját exception dobása
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto)
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
