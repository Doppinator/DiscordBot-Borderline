using System;
using Google.Apis.Sheets.v4;
using System.IO;
using Google.Apis.Auth.OAuth2;

namespace Watermelon.GoogleSheetAPI
{
    class googleAuth
    {
    static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
    static readonly string ApplicationName = "borderline-bot";   
    static readonly string SpreadSheetId = "1LrJ5UsAGKZLilHpl3c89WOQVXnOJcyOgXDaJMWSv-VA";
    // static readonly string sheet = "rdrelease";
    static SheetsService service;

    //Initialise global scope strings for storing cell data
    public string _catNum = string.Empty;
    public string _artistName = string.Empty;
    public string _title = string.Empty;
    public string _releaseDate = string.Empty;
    public string _mixes = string.Empty;
    public string _socialShare = string.Empty;
    public string _imgThumb = string.Empty;

        public void sheetAuth()
        {
            GoogleCredential credential;
            using var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read);
            {
                credential = GoogleCredential.FromStream(stream)
               .CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });    
        }

        //Read cells A2:G2 and store values in local strings
        public void getValues()
        {
            var range = "A2:G2";
            var request = service.Spreadsheets.Values.Get(SpreadSheetId, range);
            var response = request.Execute();
            var values = response.Values;               

            foreach (var row in values)
            {
                _catNum = (string)row[0];
                _artistName = (string)row[1];
                _title = (string)row[2];
                _releaseDate = (string)row[3];
                _mixes = (string)row[4];
                _socialShare = (string)row[5];
                _imgThumb = (string)row[6];
            }

            /*Pass data into global scope
            string catNum = _catNum;
            string artistName = _artistName;
            string title = _title;
            string releaseDate = _releaseDate;
            string mixes = _mixes;
            string socialShare = _socialShare;
            string imgThum = _imgThumb;
            */
        }
    }   
};