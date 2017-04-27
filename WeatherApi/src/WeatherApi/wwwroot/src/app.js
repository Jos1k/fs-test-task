import 'bootstrap';
import '.\\styles\\style.scss';

import angular from 'angular';
import uirouter from 'angular-ui-router';
import routing from '.\\app.config';
import HomeController from '.\\controllers\\homeController';

angular.module('app', [uirouter])
    .config(routing)
    .controller('homeController', HomeController);
