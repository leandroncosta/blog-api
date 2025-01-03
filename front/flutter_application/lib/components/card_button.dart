import 'package:flutter/material.dart';

class CardButton extends StatelessWidget {
  final String text;
  final VoidCallback onpressed;
  const CardButton({super.key, required this.text, required this.onpressed});

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      style: ButtonStyle(
       // backgroundColor: ,
       // backgroundColor: WidgetStateProperty.all(Colors.blue),
        shape: WidgetStateProperty.all(
          const RoundedRectangleBorder(
            borderRadius: BorderRadius.only(
              bottomLeft: Radius.circular(10),
              topRight: Radius.circular(10),
            ),
          ),
        ),
        padding: WidgetStateProperty.all(
            const EdgeInsets.symmetric(horizontal: 20, vertical: 12)),
      ),
      child: Text(style: const TextStyle(color: Colors.white), text),
      onPressed: () {
        onpressed();
      },
    );
  }
}
