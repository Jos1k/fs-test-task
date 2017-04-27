const Webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const Path = require('path');

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
            }),
            new Webpack.ProvidePlugin({
                $: "jquery",
                jQuery: "jquery"
            })
        ],
        module: {
            rules: [
                { test: /\.html$/, loader: "html-loader" },
                {
                    test: /\.scss$/,
                    use: ['style-loader', 'css-loader', 'sass-loader']
                },
                {
                    test: /\.js$/,
                    exclude: /(node_modules|bower_components)/,
                    use: {
                        loader: 'babel-loader'
                    }
                }
            ]
        },
        devServer: {
            contentBase: Path.resolve(__dirname + '\\wwwroot\\src'),
            port: 8083
        }
    };
};