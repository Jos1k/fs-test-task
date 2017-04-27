import "easy-autocomplete";
import "easy-autocomplete/dist/easy-autocomplete.css";

class HomeController {
    constructor($scope,$http) {
        const that = this;
        this.$http = $http;
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
        });
    }

    getForecast(lat,long) {
        return this.$http({
            method: 'GET',
            url: `/api/forecast?latitude=${lat}&longitude=${long}&source=WORLD_WEATHER`
        });
    }
}

export default HomeController;