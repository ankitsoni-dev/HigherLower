namespace HigherLower
{
    public class InputParser
    {
        public bool ParseScoreResponse(string response, out string parsedResponse)
        {
            parsedResponse = string.Empty;

            if (response.ToLower() == "h" || response.ToLower() == "higher")
            {
                parsedResponse = "h";
                return true;
            } 
            
            else if (response.ToLower() == "l" || response.ToLower() == "lower")
            {
                parsedResponse = "l";
                return true;
            }

            return false;
        }
    }
}
