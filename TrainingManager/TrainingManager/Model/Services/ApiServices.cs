using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.Model.Services
{
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _client;

        public ApiServices(string baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            LogHandler.Instance.Nlog.Info("Api service initialized.");
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
                LogHandler.Instance.Nlog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<WeightWorkoutDTO>> GetRecentWeightWorkoutsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts/GetRecentWorkouts");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<WeightWorkoutDTO>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<(Muscle muscle, double weight)>> GetWeeklyMuscleDataAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts/GetThisweekWeightsByMuscle");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<(Muscle muscle, double weight)>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<(WeightActivityDTO activity, double weight, int reps)>> GetMaxMovedWeightsByActivites()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts/GetMaxMovedWeightsByActivites");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<(WeightActivityDTO activity, double weight, int reps)>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
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
        public async Task<IEnumerable<WeightActivityDTO>> GetWeightActivitiesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/WeightActivities");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<ICollection<WeightActivityDTO>>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //IMAGES
        public async Task<bool> UploadProfilePicture(byte[] image)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Profile", image);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to upload profile picture. {ex.Message}");
            }
        }

        public async Task<byte[]> DownloadProfilePictureAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Account/OriginalProfile");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<byte[]>();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to upload profile picture. {ex.Message}");
            }
        }

        public async Task<string> GetNameOfTheUser()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/Account/NameOfTheUser");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    throw new Exception("Server respond is not success.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to get user name. {ex.Message}");
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

        //ACCOUNT
        public async Task<bool> LoginAsync(string userName, string password)
        {
            var accountDetails = new UserLoginDTO
            {
                UserName = userName,
                Password = password
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(accountDetails));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _client.PostAsync("api/Account/Login", content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return false;

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterAync(string name, string userName, string userEmail, string password, string confirmPassword)
        {
            var accountDetails = new UserRegistrationDTO
            {
                Name = name,
                UserName = userName,
                Email = userEmail,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(accountDetails));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("api/Account/Register", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Signout", "");

            if (response.IsSuccessStatusCode)
                return true;

            throw new Exception($"Service returned response: {response.StatusCode}");
        }

        //STATS
        public async Task<IEnumerable<(int year, int month, double weight)>> GetMovedWorkoutsGroupByMonth()
        {
            HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts/MovedWeightsByMonth");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IEnumerable<(int year, int month, double weight)>>();
            else
                throw new Exception("Server respond is not success.");
        }

        public async Task<IEnumerable<(int year, int month, IEnumerable<(DateTime date, double weight)>)>> GetMovedWeightsGroupByMonth()
        {
            HttpResponseMessage response = await _client.GetAsync("api/WeightWorkouts/GetMovedWeightsGroupByMonth");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IEnumerable<(int year, int month, IEnumerable<(DateTime date, double weight)>)>>();
            else
                throw new Exception("Server respond is not success.");
        }

        public async Task<IEnumerable<(DateTime date, double weight)>> GetMovedWeightsInTheMonth(int year, int month)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/WeightWorkouts/MovedWeightsInTheMonth/{year}/{month}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IEnumerable<(DateTime date, double weight)>>();
            else
                throw new Exception("Server respond is not success.");
        }
    }
}
