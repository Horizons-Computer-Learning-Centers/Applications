import { Component } from '@angular/core';
import { WeatherForecastClient } from './shared/api/weather-api';

@Component({
  selector: 'horizons-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'horizons';
  weather: any;

  constructor(private readonly weatherService: WeatherForecastClient) {
    this.weatherService.weatherForecast().subscribe(result => {
      this.weather = result;
    })
  }
}
