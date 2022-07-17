var path = require('path'),
    express = require('express'),
    app = express()

var appDirectory = __dirname
app.use(express.static(appDirectory))
console.log('server using static ' + appDirectory)

app.use(function(req, res, next) {
  res.status(404).sendFile(path.join(appDirectory, 'index.html'))
})

var port = process.env.PORT || 3000;
app.listen(port)
console.log('server listening on port ' + port)