var Webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var Path = require('path');

module.exports = () => {
    return {
        context: __dirname + "\\wwwroot\\src",
        devtool: 'inline-sourcemap',
        entry: '.\\app.js',
        output: {
            path: __dirname + "\\wwwroot\\dist",
            filename: 'app.js'
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: 'index.template.ejs',
                inject: 'body'
            })
        ],
        module: {
            rules: [
                {
                    test: /\.scss$/,
                    use: ['style-loader', 'css-loader', 'sass-loader']
                }
            ]
        },
        devServer: {
            contentBase: Path.resolve(__dirname + '\\wwwroot\\src'),
            port: 8083
        }
    };
};