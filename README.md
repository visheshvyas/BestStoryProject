# Introduction
---
There are 2 projects created
- **BestStories** : this contains code for RESTFul WebAPI service.
- **BestStoriesClient** : this contains code for client that will connect to API to fetch the required count of stories.

### How it works
---
- Download the BestStoryProject repository from the GitHub.
- In this folder you will find "StartService.bat", which can be used to launch the service.
- Once the service is launched successfully, you will see the message "***** Service Started *****"
- In the same folder you will find "StartClient.bat", which can be used to launch the client and fetch the stories.
- To test API from your own client, you can use URL "https://localhost:7219/BestStory?cnt={requestCnt}", and replace the "{requestCnt}" with number of stories you want to fetch.

### Assumptions
---
- There is no intraday update in the Story Ids and Comments.
- Comments for a Story (required for count in output) is available at 1st level kids only. 
- Service will launch at the start of day.
- If Service is restarted, in-memory data will be loaded again (this might get a new set of Story Ids and Comments).

### Task To Do
---
- Loading data intraday after service startup. In this case 2 tasks will be performed :
    -- Newly added story ids will be loaded.
    -- Comment count of existing Story Ids will be updated if required.
- Persisted logs can be generated for loading and client request.
- Exception handling at granular level.
- Performance counters to see the slowness if any.