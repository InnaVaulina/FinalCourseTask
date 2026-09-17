namespace TTB.Support
{
    public static class CatchExeption
    {
        public static async Task<T> ExecuteWithCatchAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Rethrow the exception if you want to propagate it
            }
        }
    }
}
