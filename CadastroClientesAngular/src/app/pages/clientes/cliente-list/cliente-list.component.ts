import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { ClienteService } from '../../../core/services/cliente.service';
import { Cliente } from '../../../shared/models/cliente.model';

@Component({
  selector: 'app-cliente-list',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatInputModule, MatFormFieldModule ],
  templateUrl: './cliente-list.component.html',
})
export class ClienteListComponent implements OnInit {
  clientes: Cliente[] = [];
  displayedColumns: string[] = ['nome', 'documento', 'tipoPessoa', 'email', 'acoes'];

  constructor(
    private clienteService: ClienteService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarClientes();
  }

  carregarClientes() {
    this.clienteService.listar().subscribe({
      next: (data) => {
        this.clientes = data;
      },
      error: (err) => console.error('Erro ao buscar clientes', err),
    });
  }

  editar(id?: string) {
    if (id) {
      this.router.navigate(['/cliente/editar', id]);
    }
  }

  excluir(id?: string) {
    if (id && confirm('Tem certeza que deseja excluir este cliente?')) {
      this.clienteService.deletar(id).subscribe(() => {
        this.carregarClientes();
      });
    }
  }

  novo() {
    this.router.navigate(['/clientes/novo']);
  }
}
