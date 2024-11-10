import { NgModule } from '@angular/core';

import { NavRoutingModule } from './nav-routing.module';
import { NavComponent } from './nav.component';
import { WelcomeComponent } from '../welcome/welcome.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [NavComponent, WelcomeComponent],
  imports: [SharedModule, NavRoutingModule],
})
export class NavModule {}
