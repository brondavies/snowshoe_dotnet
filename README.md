Snowshoe API with .NET client
============

The Snowshoe client submits point data to the Snowshoe API for authentication. The client will return a JSON object, containing either the serial of the matched stamp (a success!) or an error.

## Dependencies
- https://github.com/danielcrenna/oauth/ or any other oauth implementation that can generate an oauth 1.0 Authorization header

## Installation
Open SLN file and run

## Usage: Setting up the client and making a POST

Your Snowshoe APP_KEY and APP_SECRET must be updated in Web.Debug.config and Web.Release.config according to the environment where it will run.

Currently, all posts are made to v2 of the API at http://beta.snowshoestamp.com/api/v2/stamp.

Below are examples of success and error JSON responses from the API.

```javascript
// Success
{
  "stamp": {
    "serial": "DEV-STAMP"
  },
  "receipt": "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
  "secure": false,
  "created": "2015-03-24 11:27:33.014149"
}

// Error
{
  "error": {
    "message": "Stamp not found",
    "code": 32
    },
    "receipt": "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
    "secure": false,
    "created": "2015-03-24 11:27:48.235046"
}
```

## License
MIT (see LICENSE file)