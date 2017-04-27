import "easy-autocomplete";
import "easy-autocomplete/dist/easy-autocomplete.min.css";
import "bootstrap-select";
import "bootstrap-select/dist/css/bootstrap-select.min.css";

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
                url:function(phrase) {
                    return `/api/cities/search?byName=${phrase}`;
                },
                getValue: function(element) {
                    return element.structured_formatting.main_text;
                },
                listLocation: "predictions",
                list: {
                    onChooseEvent: function() {
                        const placeId = $('#cities-autocomplete').getSelectedItemData().place_id;
                        that.$http({
                            method: 'GET',
                            url: `/api/cities/${placeId}`
                        }).then(function (selectedCity) {
                            const location = selectedCity.data.result.geometry.location;
                            that.getForecast(location.lat, location.lng).then(function(forecast) {
                                that.forecast = forecast.data;
                                that.selectedCity = selectedCity.data.result;
                            });
                        });
                    }
                }
            });
            $('#forecast-source').selectpicker();
        });
    }

    getForecast(lat,long) {
        return this.$http({
            method: 'GET',
            url: `/api/forecast?latitude=${lat}&longitude=${long}&source=${this.selectedSource}`
        });
    }
}

export default HomeController;