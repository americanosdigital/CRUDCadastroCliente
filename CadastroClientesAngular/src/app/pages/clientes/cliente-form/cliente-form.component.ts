import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { ClienteService } from '../../../core/services/cliente.service';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './cliente-form.component.html',
})
export class ClienteFormComponent implements OnInit {
  form!: FormGroup;
  clienteId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.clienteId = this.route.snapshot.paramMap.get('id');

    this.form = this.fb.group({
      nome: ['', [Validators.required]],
      documento: ['', [Validators.required]],
      tipoPessoa: ['', [Validators.required]],
      dataNascimento: ['', [Validators.required]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      inscricaoEstadual: [''],
      isento: [false],
      endereco: this.fb.group({
        cep: ['', Validators.required],
        logradouro: ['', Validators.required],
        numero: [''],
        bairro: ['', Validators.required],
        cidade: ['', Validators.required],
        estado: ['', [Validators.required, Validators.maxLength(2)]],
      }),
    });

    if (this.clienteId) {
      this.clienteService.obterPorId(this.clienteId).subscribe(cliente => {
        this.form.patchValue(cliente);
      });
    }
  }

  salvar(): void {
    if (this.form.invalid) return;

    const cliente = this.form.value;

    if (this.clienteId) {
      this.clienteService.atualizar(this.clienteId, cliente).subscribe(() => {
        this.router.navigate(['/clientes']);
      });
    } else {
      this.clienteService.criar(cliente).subscribe(() => {
        this.router.navigate(['/clientes']);
      });
    }
  }

  cancelar(): void {
    this.router.navigate(['/clientes']);
  }
}
