**Simple Discord Bot for Label Groups**

**User interaction:**
Available text iteractions:

hello
ping
register

**Admin commands**

CTA
- "Call to Action" - Invite all users to share the latest label release & provide a link for sharing.

Uses the Google Sheets API to authenticate the bot with Google, retrieve the details of the latest label release & assets from a Google Sheet, store release details as local variables e.g.

string releaseID = getReleaseInfo._catNum;

            string artistName = getReleaseInfo._artistName;
            
            string releaseName = getReleaseInfo._title;
            
            string releaseDate = getReleaseInfo._releaseDate;
            
            string remixerName = getReleaseInfo._mixes;
            
            string socialShare = getReleaseInfo._socialShare;
            
            string releaseArtwork = getReleaseInfo._imgThumb;
            
            string labelID = "<@&908909123196841994>, ";
            

Embeds the retrieved release details in a new post and tags all group users with details for sharing.
