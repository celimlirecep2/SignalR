import { Component, OnInit } from '@angular/core';
import { CovidService } from './Services/covid.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {


  title = 'SignalRUIApp';
  columnNames = ["Tarih", "İstanbul", "Ankara", "İzmir", "Konya", "Antalya"];
  public ChartType = ChartType.LineChart;
  options: any = { legend: { position: "Bottom" } };
  constructor(public covidService: CovidService) { }
  ngOnInit(): void {
    this.covidService.startConnection();
    this.covidService.startListener();

  }

}
enum ChartType {
  Histogram = 'Histogram',
  Line = 'Line',
  LineChart = 'LineChart'
}
