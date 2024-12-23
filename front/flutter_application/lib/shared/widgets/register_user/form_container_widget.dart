import 'dart:convert';
import 'dart:developer';
import 'package:flutter/material.dart';
import 'package:flutter_application/shared/services/api_service.dart';
import 'package:flutter_application/shared/widgets/register_user/button_register_widget.dart';
import 'package:flutter_application/shared/widgets/register_user/input_widget.dart';

class FormContainerWidget extends StatefulWidget {
  const FormContainerWidget({super.key});

  @override
  State<FormContainerWidget> createState() {
    return _FormContainerWidgetState();
  }
}

class _FormContainerWidgetState extends State<FormContainerWidget> {
  final ApiService _apiService = ApiService();
  final TextEditingController _username = TextEditingController();
  final TextEditingController _password = TextEditingController();

  void _registerUser() async {
    try {
      final response = await _apiService.createUser(
        _username.text,
        _password.text,
      );

      log('Usuário criado: ${const JsonEncoder.withIndent(" ").convert(response)}');
    } catch (e) {
      log('Erro ao criar usuário: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;

    return Expanded(
      flex: 2,
      child: Container(
        decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.only(
                topLeft: Radius.circular(50), topRight: Radius.circular(50))),
        width: size.width,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            InputWidget(label: "Usuário", controller: _username),
            const SizedBox(height: 20),
            InputWidget(
              label: "Senha",
              controller: _password,
            ),
            const SizedBox(
              height: 100,
            ),
            ButtonRegisterWidget(label: "Criar usuário", onTap: _registerUser)
          ],
        ),
      ),
    );
  }
}
