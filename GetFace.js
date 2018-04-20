var https = require('https');
var qs = require('querystring');

const param = qs.stringify({
    'grant_type': 'client_credentials',
    'client_id': 'y2EcSosAzxyGH5EZQnjc9S86',
    'client_secret': 'wdspPGwbdGqPN8of96CB8gM048Ft8TZF'
});

https.get(
    {
        hostname: 'aip.baidubce.com',
        path: '/oauth/2.0/token?' + param,
        agent: false
    },
    function (err, res) {
        // 在标准输出中查看运行结果
        var response;
        res.pipe(process.stdout);
    }
);
