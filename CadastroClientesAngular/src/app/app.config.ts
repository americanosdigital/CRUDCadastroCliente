import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';  
import { routes } from './app.routes'; 
import { ClienteFormComponent } from './pages/clientes/cliente-form/cliente-form.component';
import { ClienteListComponent } from './pages/clientes/cliente-list/cliente-list.component';

export const appConfig: ApplicationConfig = {
  providers: [
    
    provideRouter(routes), 
    importProvidersFrom(
      HttpClientModule,    
      ReactiveFormsModule, 
      RouterModule.forRoot(routes)
    )
  ],
};
