import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import appSelectors from '../../../../^state/app.selectors';

@Component({
  selector: 'horizons-spinner',
  template: `
    <div class="app-loading" *ngIf="spinner$ | async">
      <div class="logo">
        <svg class="spinner" viewBox="25 25 50 50">
          <circle
            class="path"
            cx="50"
            cy="50"
            r="20"
            fill="none"
            stroke-width="3.5"
            stroke-miterlimit="10"
          />
        </svg>
      </div>
    </div>
  `,
  styles: [
    `
      .app-loading {
        position: relative;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        height: 100%;
      }

      .app-loading .spinner {
        height: 120px;
        width: 120px;
        animation: rotate 2s linear infinite;
        transform-origin: center center;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
      }

      .app-loading .spinner .path {
        stroke-dasharray: 1, 200;
        stroke-dashoffset: 0;
        animation: dash 1.5s ease-in-out infinite;
        stroke-linecap: round;
        stroke: #5c60f5;
      }

      @keyframes rotate {
        100% {
          transform: rotate(360deg);
        }
      }

      @keyframes dash {
        0% {
          stroke-dasharray: 1, 200;
          stroke-dashoffset: 0;
        }
        50% {
          stroke-dasharray: 89, 200;
          stroke-dashoffset: -35px;
        }
        100% {
          stroke-dasharray: 89, 200;
          stroke-dashoffset: -124px;
        }
      }
    `,
  ],
})
export class SpinnerComponent {
  spinner$ = this.store.select(appSelectors.spinnerStatus);

  constructor(private readonly store: Store) {}
}
