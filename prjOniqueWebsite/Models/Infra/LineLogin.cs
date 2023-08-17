namespace prjOniqueWebsite.Models.Infra
{
    public static class LineLogin
    {
        public class LineLoginToken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }

        public class LineProfile
        {
            public string userId { get; set; }
            public string displayName { get; set; }
            public string pictureUrl { get; set; }
            public string statusMessage { get; set; }
        }
    }
}
