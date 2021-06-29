import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  "scripts": [

    "../node_modules/1js/popper.min.js",
    "../node_modules/1js/plugins.js",
    "../node_modules/1js/main.js",
    "../node_modules/jquery/dist/jquery.js"
  ]
}
