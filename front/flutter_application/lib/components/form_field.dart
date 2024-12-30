import 'package:flutter/material.dart';

class FormFieldInput extends StatelessWidget {
  TextEditingController controller;
  String labelText;
  FormFieldInput(
      {super.key,
      required TextEditingController this.controller,
      required String this.labelText});

  @override
  Widget build(BuildContext context) {
    return TextFormField(
      controller: controller,
      decoration:  InputDecoration(
        border: const OutlineInputBorder(borderRadius: BorderRadius.all(Radius.circular(10))),
        labelText: labelText,
      ),
      onChanged: (value) {
        controller.text = value;
      },
    );
  }
}
