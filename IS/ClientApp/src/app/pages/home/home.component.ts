import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  test() {
    this.http.get('/api/files')
      .subscribe((data: any) => {
        console.log(data);
      }, error => {
        console.log(error);
      });
  }

}
