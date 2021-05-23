using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.ApiService
{
    public class APIConnectionService
    {
        private HttpClient _client;

        public APIConnectionService(string baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<WeigthExerciseDTO>> GetWeightExercisesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/WeightExercises");

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<WeigthExerciseDTO> exercises = await response.Content.ReadAsAsync<IEnumerable<WeigthExerciseDTO>>();
            }
        }
    }
}
