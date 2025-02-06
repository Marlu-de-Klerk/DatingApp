import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 
import { CommonModule, NgFor } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  
  http = inject(HttpClient);
  title = 'Dating App';
  users: any;
  
  ngOnInit(): void {
    this.http.get('https://Localhost:5001/API/user').subscribe ({
    next: Response => this.users = Response,
    error: error => console.log(error),
    complete: () => console.log('Request has completed')
    })
  }
}
