import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
}

const hamburger = document.querySelector(".icon-menu");
const navMenu = document.querySelector(".nav-content");

hamburger?.addEventListener("onclick", mobileMenu);

export function mobileMenu() {
  function warnUser(): void {
    alert("This is my warning message");
}}