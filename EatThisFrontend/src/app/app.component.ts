import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private renderer2: Renderer2,
    @Inject(DOCUMENT) private document: Document
    ){}

  ngOnInit(): void {
    this.authService.autoLogIn();
    let script = this.renderer2.createElement('script');
    script.src = `https://kit.fontawesome.com/0fdcba151e.js`

    this.renderer2.appendChild(this.document.body, script);
  }

  title = 'Zjedz to';
}
