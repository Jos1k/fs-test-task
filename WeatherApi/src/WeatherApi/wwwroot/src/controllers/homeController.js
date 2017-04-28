import "easy-autocomplete";
import "easy-autocomplete/dist/easy-autocomplete.min.css";
import "bootstrap-select";
import "bootstrap-select/dist/css/bootstrap-select.min.css";
import moment from "moment";

class HomeController {
    constructor($scope,$http) {
        const that = this;
        this.$http = $http;
        this.sources = [
            { value:'WORLD_WEATHER',name:'World Weather' },
            { value:'FORECAST_IO',name:'Forecast IO'}
        ];
        this.selectedSource = this.sources[0].value;

        $scope.$on('$viewContentLoaded', function() {
            $('#cities-autocomplete').easyAutocomplete({
                url: (phrase) => { return `/api/cities/search?byName=${phrase}`; },
                getValue: (element) => { return element.structured_formatting.main_text; },
                listLocation: "predictions",
                list: { onChooseEvent: function() {
                    that.selectedCityId = $('#cities-autocomplete').getSelectedItemData().place_id;
                    that.loadForecast.apply(that);
                } }
            });
            $('#forecast-source').selectpicker();
            $('#forecast-source').on('change', function(){ that.loadForecast.apply(that); });
        });
    }

    formatCurrentWeather(currentWeatherData) {
        const date = moment(currentWeatherData.date);
        currentWeatherData.weekday = date.format('dddd');
        currentWeatherData.date = date.format('MMMM Do');
        
        if (currentWeatherData.temperature) {

            currentWeatherData.temperature = parseInt(currentWeatherData.temperature);
            currentWeatherData.apparentTemperature = parseInt(currentWeatherData.apparentTemperature);

        } else if (currentWeatherData.temperatureMin && currentWeatherData.temperatureMax) {

            currentWeatherData.temperatureMin = parseInt(currentWeatherData.temperatureMin);
            currentWeatherData.apparentTemperatureMin = parseInt(currentWeatherData.apparentTemperatureMin);
            
            currentWeatherData.temperatureMax = parseInt(currentWeatherData.temperatureMax);
            currentWeatherData.apparentTemperatureMax = parseInt(currentWeatherData.apparentTemperatureMax);
        }
       
        currentWeatherData.humidity = parseFloat(currentWeatherData.humidity).toFixed(2);
        return currentWeatherData;
    }

    loadForecast() {
        const that = this;
        if (this.selectedCityId) {
            this.$http({
                method: 'GET',
                url: `/api/cities/${this.selectedCityId}`
            }).then(function (selectedCity) {
                const location = selectedCity.data.result.geometry.location;
                that.getForecast(location.lat, location.lng).then(function(forecast) {
                    that.forecast = forecast.data.futureForecasts.map(function(obj) {
                        return that.formatCurrentWeather(obj);
                    });
                    that.selectedCity = selectedCity.data.result;
                    that.todayForecast = that.formatCurrentWeather(forecast.data.currently);
                    that.selectedForecast = that.todayForecast;

                    $('html, body').animate({
                        scrollTop: $("#forecastSelectedCardAnchor").offset().top
                    }, 1000);
                });
            });
        }
    }

    selectForecast(selectedForecast) {
        this.selectedForecast = selectedForecast;
    }

    getForecast(lat,long) {
        return this.$http({
            method: 'GET',
            url: `/api/forecast?latitude=${lat}&longitude=${long}&source=${this.selectedSource}`
        });
    }
}

export default HomeController;