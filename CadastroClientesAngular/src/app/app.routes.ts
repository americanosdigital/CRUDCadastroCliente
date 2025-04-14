import { Routes } from '@angular/router';
import { ClienteFormComponent } from './pages/clientes/cliente-form/cliente-form.component';
import { ClienteListComponent } from './pages/clientes/cliente-list/cliente-list.component';

export const routes: Routes = [
  { path: '', component: ClienteListComponent },
  { path: 'clientes', component: ClienteListComponent },
  { path: 'clientes/novo', component: ClienteFormComponent },
  { path: 'cliente/editar/:id', component: ClienteFormComponent },
];

