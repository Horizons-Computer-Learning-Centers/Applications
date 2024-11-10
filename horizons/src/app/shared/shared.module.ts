import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { SidenavComponent } from './components/layout/sidenav/sidenav.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { FooterMinimalComponent } from './components/layout/footer-minimal/footer-minimal.component';
import { HeaderMinimalComponent } from './components/layout/header-minimal/header-minimal.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { LogoComponent } from './components/layout/logo/logo.component';
import { LogoMinimalComponent } from './components/layout/logo-minimal/logo-minimal.component';
import { RouterModule } from '@angular/router';
import { TopComponent } from './components/layout/header/top/top.component';
import { BottomComponent } from './components/layout/header/bottom/bottom.component';
import { MastComponent } from './components/layout/header/mast/mast.component';
import { SpinnerComponent } from './components/app/spinner/spinner.component';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    SidenavComponent,
    FooterComponent,
    FooterMinimalComponent,
    HeaderMinimalComponent,
    HeaderComponent,
    LogoComponent,
    LogoMinimalComponent,
    TopComponent,
    BottomComponent,
    MastComponent,
    SpinnerComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,
    ToastrModule.forRoot(),
  ],
  exports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    FooterComponent,
    FooterMinimalComponent,
    HeaderMinimalComponent,
    HeaderComponent,
    LogoComponent,
    LogoMinimalComponent,
    SpinnerComponent,
    ToastrModule,
  ],
})
export class SharedModule {}
