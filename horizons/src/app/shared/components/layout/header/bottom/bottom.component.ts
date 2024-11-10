import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'horizons-bottom',
  templateUrl: './bottom.component.html',
  styleUrls: ['./bottom.component.scss'],
})
export class BottomComponent {
  scrollPosition: number = 0;

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: any) {
    this.scrollPosition =
      window.pageYOffset || document.documentElement.scrollTop;
    console.log(this.scrollPosition);
  }
}
