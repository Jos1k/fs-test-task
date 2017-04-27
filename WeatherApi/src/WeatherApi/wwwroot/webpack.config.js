const Webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const Path = require('path');

module.exports = () => {
    return {
        context: __dirname + "\\src\\",
        devtool: 'inline-sourcemap',
        entry: '.\\app.js',
        output: {
            path: __dirname + "\\dist\\",
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
        }
    };
};